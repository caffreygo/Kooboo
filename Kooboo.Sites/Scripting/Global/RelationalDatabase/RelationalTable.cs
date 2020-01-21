﻿using KScript;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kooboo.Sites.Scripting.Global.RelationalDatabase
{
    public class RelationalTable<TExecuter, TSchema> : ITable where TExecuter : SqlExecuter where TSchema : RelationalSchema
    {
        readonly static object _locker = new object();
        RelationalSchema _schema;

        public string Name { get; set; }
        public RelationalDatabase<TExecuter, TSchema> Database { get; set; }

        public RelationalTable(string name, RelationalDatabase<TExecuter, TSchema> database)
        {
            Database = database;
            Name = name;
        }

        void TryUpgradeSchema(IDictionary<string, object> value)
        {
            lock (_locker)
            {
                var newItems = EnsureSchemaCompatible(value);

                if (newItems.Count() > 0)
                {
                    _schema.AddItems(newItems);
                    Database.SqlExecuter.UpgradeSchema(Name, newItems);
                }
            }
        }

        List<RelationalSchema.Item> EnsureSchemaCompatible(IDictionary<string, object> value)
        {
            var newSchema = (TSchema)Activator.CreateInstance(typeof(TSchema), value);
            var compatible = _schema.Compatible(newSchema, out var newItems);
            if (!compatible) throw new SchemaNotCompatibleException();
            return newItems;
        }

        void EnsureTableCreated()
        {
            lock (_locker)
            {
                if (_schema == null) _schema = Database.SqlExecuter.GetSchema(Name);
                if (!_schema.Created) Database.SqlExecuter.CreateTable(Name);
            }
        }

        object EnsureHaveId(IDictionary<string, object> value, string id = null)
        {
            if (id == null) id = Guid.NewGuid().ToString();
            if (!value.ContainsKey("_id")) value.Add("_id", id);
            return value;
        }

        /// <summary>
        /// because we can't know null field type
        /// </summary>
        /// <param name="value"></param>
        void ClearNullField(IDictionary<string, object> value)
        {
            foreach (var item in value.Where(w => w.Value == null).Select(s => s.Key).ToArray())
            {
                value.Remove(item);
            }
        }

        public object add(object value)
        {
            var dic = kHelper.CleanDynamicObject(value);
            ClearNullField(dic);
            EnsureTableCreated();
            TryUpgradeSchema(dic);
            var newId = Guid.NewGuid().ToString();
            EnsureHaveId(dic, newId);
            Database.SqlExecuter.Insert(Name, dic);
            return newId;
        }

        public IDynamicTableObject[] all()
        {
            EnsureTableCreated();
            var data = Database.SqlExecuter.QueryData(Name);
            return RelationalDynamicTableObject<TExecuter, TSchema>.CreateList(data.Select(s => s as IDictionary<string, object>).ToArray(), this);
        }

        public object append(object value)
        {
            var dic = kHelper.CleanDynamicObject(value);
            EnsureTableCreated();
            EnsureSchemaCompatible(dic);
            var newId = Guid.NewGuid().ToString();
            EnsureHaveId(dic, newId);
            Database.SqlExecuter.Append(Name, dic, _schema);
            return newId;
        }

        public void createIndex(string fieldname)
        {
            EnsureTableCreated();
            Database.SqlExecuter.CreateIndex(Name, fieldname);
        }

        public void delete(object id)
        {
            EnsureTableCreated();
            Database.SqlExecuter.Delete(Name, id.ToString());
        }

        public IDynamicTableObject find(string query)
        {
            EnsureTableCreated();
            var data = Database.SqlExecuter.QueryData(Name, query).FirstOrDefault();
            return RelationalDynamicTableObject<TExecuter, TSchema>.Create(data as IDictionary<string, object>, this);
        }

        public IDynamicTableObject find(string fieldName, object matchValue)
        {
            EnsureTableCreated();
            var data = Database.SqlExecuter.QueryData(Name, $"{fieldName} == '{matchValue}'").FirstOrDefault();
            return RelationalDynamicTableObject<TExecuter, TSchema>.Create(data as IDictionary<string, object>, this);
        }

        public IDynamicTableObject[] findAll(string query)
        {
            EnsureTableCreated();
            var data = Database.SqlExecuter.QueryData(Name, query);
            return RelationalDynamicTableObject<TExecuter, TSchema>.CreateList(data.Select(s => s as IDictionary<string, object>).ToArray(), this);
        }

        public IDynamicTableObject[] findAll(string field, object value)
        {
            EnsureTableCreated();
            var data = Database.SqlExecuter.QueryData(Name, $"{field} == '{value}'");
            return RelationalDynamicTableObject<TExecuter, TSchema>.CreateList(data.Select(s => s as IDictionary<string, object>).ToArray(), this);
        }

        public IDynamicTableObject get(object id)
        {
            EnsureTableCreated();
            var data = Database.SqlExecuter.QueryData(Name, $"_id == '{id}'").FirstOrDefault();
            return RelationalDynamicTableObject<TExecuter, TSchema>.Create(data as IDictionary<string, object>, this);
        }

        public ITableQuery Query()
        {
            return new RelationalTableQuery<TExecuter, TSchema>(this);
        }

        public ITableQuery Query(string query)
        {
            var result = new RelationalTableQuery<TExecuter, TSchema>(this);
            result.Where(query);
            return result;
        }

        public void update(object newvalue)
        {
            var dic = kHelper.CleanDynamicObject(newvalue);
            if (dic.ContainsKey("_id")) update(dic["_id"], dic);
            else add(dic);
        }

        public void update(object id, object newvalue)
        {
            var dic = kHelper.CleanDynamicObject(newvalue);
            ClearNullField(dic);
            if (dic.ContainsKey("_id")) dic.Remove("_id");
            EnsureTableCreated();
            TryUpgradeSchema(dic);
            Database.SqlExecuter.UpdateData(Name, id.ToString(), dic);
        }
    }
}
