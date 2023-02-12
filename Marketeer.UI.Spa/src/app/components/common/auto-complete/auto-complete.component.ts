import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  fromEvent,
  map,
  of,
} from 'rxjs';

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss'],
})
export class AutoCompleteComponent implements OnInit {
  @Input()
  label = '';
  @Input()
  class = '';
  @Input() set searchStr(value: string | null | undefined) {
    if (value == undefined) {
      this.searchText = null;
    } else {
      this.searchText = value;
    }
  }
  @Input()
  displayProperty: string | null = null;
  @Input()
  valueProperty: string | null = null;
  options = [] as any[];
  @Input() set searchResults(items: any[]) {
    this.options = items;
    this.inProgress = false;
  }

  @Output()
  search = new EventEmitter<string>();
  @Output()
  selected = new EventEmitter<any | null>();

  @ViewChild('autoSearchInput', { static: true })
  autoSearchInput!: ElementRef;

  inProgress = false;
  ctrl!: FormControl;
  searchText: string | null = null;

  constructor() {}

  ngOnInit(): void {
    this.ctrl = new FormControl();
    fromEvent(this.autoSearchInput.nativeElement, 'keyup')
      .pipe(
        map((event: any) => event.target.value),
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe((text: string) => {
        this.searchText = text;
        this.inProgress = true;
        this.search.emit(text);
      });
  }

  onSelection(event: MatAutocompleteSelectedEvent): void {
    this.selected.emit(this.getOptionValue(event.option.viewValue));
  }

  getOptionDisplay(option: any): any {
    if (this.displayProperty == null || this.displayProperty.length == 0) {
      return option;
    }
    const a = Object.keys(option);
  }

  getOptionValue(option: any): any {
    if (this.displayProperty == null || this.displayProperty.length == 0) {
      return option;
    }
    const a = Object.keys(option);
  }
}
