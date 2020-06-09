import { Injectable, RendererFactory2, Inject, ViewEncapsulation } from '@angular/core';
import { DOCUMENT } from '@angular/common';
@Injectable()
export abstract class ILinkeService {
  abstract AddTag(tag: LinkDefinition): void;
  abstract RemoveTag(id: string): void;
}
@Injectable()
export class LinkService implements ILinkeService {

  constructor(
    private rendererFactory: RendererFactory2,
    @Inject(DOCUMENT) private document
  ) { }
  RemoveTag(id: string): void {
    throw new Error("Method not implemented.");
  }

  AddTag(tag: LinkDefinition): void {
    try {
      const renderer = this.rendererFactory.createRenderer(this.document, {
        id: '-1',
        encapsulation: ViewEncapsulation.None,
        styles: [],
        data: {}
      });

      const link = renderer.createElement('link');
      const head = this.document.head;
      const selector = this._parseSelector(tag);

      if (head === null) {
        throw new Error('<head> not found within DOCUMENT.');
      }

      Object.keys(tag).forEach((prop: string) => {
        return renderer.setAttribute(link, prop, tag[prop]);
      });

      // [TODO]: get them to update the existing one (if it exists) ?
      renderer.appendChild(head, link);

    } catch (e) {
      console.error('Error within linkService : ', e);
    }
  }
  private _parseSelector(tag: LinkDefinition): string {
    // Possibly re-work this
    const attr: string = tag.rel ? 'rel' : 'hreflang';
    return `${attr}="${tag[attr]}"`;
  }
}


export declare type LinkDefinition = {
  id: string
  charset?: string;
  crossorigin?: string;
  href?: string;
  hreflang?: string;
  media?: string;
  rel?: string;
  rev?: string;
  sizes?: string;
  target?: string;
  type?: string;
  title?: string;
  disabled?: string
} & {
  [prop: string]: string;
};