import { MenuItem, createItem } from "../basic";
import { TEXT } from "@/common/lang";
import { MenuActions } from "@/events/FloatMenuClickEvent";
import { canJump } from "@/dom/utils";
import context from "@/common/context";
import { getPageId } from "@/kooboo/utils";
import qs from "query-string";

export function createJumpLinkItem(): MenuItem {
  const { el, setVisiable } = createItem(TEXT.JUMP_LINK, MenuActions.jumpLink);

  const update = () => {
    setVisiable(true);
    let args = context.lastHoverDomEventArgs;
    if (!canJump(args.element)) setVisiable(false);
  };

  el.addEventListener("click", e => {
    let url = context.lastHoverDomEventArgs.element.getAttribute("href")!;
    let pageId = getPageId();
    let parsed = qs.parse(parent.location.search);
    let query = {
      SiteId: parsed["SiteId"],
      accessToken: parsed["accessToken"],
      pageId: pageId,
      pageUrl: url
    };

    parent.location.href = parent.location.origin + parent.location.pathname + "?" + qs.stringify(query);
  });

  return {
    el,
    update
  };
}
