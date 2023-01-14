import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss'],
})
export class AutoCompleteComponent implements OnInit {
  @Input()
  displayProperty: string | null = null;
  @Input()
  items = [];

  @Output()
  search = new EventEmitter<string>();
  @Output()
  select = new EventEmitter<any | null>();

  searchStr: Observable<any | null> | null = null;

  constructor() {}

  ngOnInit(): void {}
}
