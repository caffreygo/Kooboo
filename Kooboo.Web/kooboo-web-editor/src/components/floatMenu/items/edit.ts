import { TEXT } from "@/common/lang";
import { MenuActions } from "@/events/FloatMenuClickEvent";
import context from "@/common/context";
import { MenuItem, createItem } from "../basic";
import { isDynamicContent } from "@/kooboo/utils";
import { isBody } from "@/dom/utils";
import { setInlineEditor } from "@/components/richEditor";
import { getMenuComment, getEditComment } from "../utils";
import { KoobooComment } from "@/kooboo/KoobooComment";

export function createEditItem(): MenuItem {
  const { el, setVisiable } = createItem(TEXT.EDIT, MenuActions.edit);
  const update = (comments: KoobooComment[]) => {
    setVisiable(true);
    let args = context.lastSelectedDomEventArgs;
    if (isBody(args.element)) return setVisiable(false);
    if (getMenuComment(comments)) return setVisiable(false);
    if (!getEditComment(comments)) return setVisiable(false);
    if (!args.koobooId) return setVisiable(false);
    var reExcept = /^(img|button|input|textarea|hr|area|canvas|meter|progress|select|tr|td|tbody|thead|tfoot|th)$/i;
    let el = args.element;
    if (reExcept.test(el.tagName)) return setVisiable(false);
    if (isDynamicContent(args.element)) return setVisiable(false);
  };

  el.addEventListener("click", () => {
    let args = context.lastSelectedDomEventArgs;
    setInlineEditor(args.element).catch(() => {});
  });

  return { el, update };
}
