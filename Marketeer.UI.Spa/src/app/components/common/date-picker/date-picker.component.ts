import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  NgbCalendar,
  NgbDate,
  NgbDateStruct,
} from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss'],
})
export class DatePickerComponent implements OnInit {
  dateStr: string | null = null;
  @Input() set date(value: Date | null | undefined) {
    if (value == undefined) {
      this.dateStr = null;
    } else {
      this.dateStr = this.datePipe.transform(value, 'yyyy-MM-dd')!;
    }
  }
  @Input()
  label = 'Date:';
  @Input()
  class = '';
  @Input()
  disabled = false;
  @Input()
  readonly = false;

  @Output()
  dateChange = new EventEmitter<Date | undefined>();

  constructor(private readonly datePipe: DatePipe) {}

  ngOnInit(): void {}

  toDate(date: string | null): Date | null {
    const m = moment(date, 'YYYY-MM-DD');
    if (m == null || !moment.isMoment(m) || !m.isValid()) {
      return null;
    }

    return m.toDate();
  }

  toStr(date: Date | null): string | null {
    if (date == null) {
      return null;
    }

    const dp = this.datePipe.transform(date, 'yyyy-MM-dd')!;
    return dp;
  }

  onBlur(): void {
    const newDate = this.toDate(this.dateStr);
    this.dateStr = this.toStr(newDate);

    if (newDate != null) {
      this.dateChange.emit(newDate);
    } else {
      this.dateChange.emit(undefined);
    }
  }
}
