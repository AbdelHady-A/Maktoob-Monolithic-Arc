import { Component, OnInit } from '@angular/core';
import { IOverlayFacade } from '../facades/overlay.facade';

@Component({
    selector: 'app-control',
    templateUrl: './control.component.html',
    styleUrls: ['./control.component.scss']
})
export class ControlComponent implements OnInit {

    constructor(private controlOverlay: IOverlayFacade) { }

    ngOnInit(): void { }

}