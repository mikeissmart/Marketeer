import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  NgbDate,
  NgbCalendar,
  NgbDateParserFormatter,
} from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

@Component({
  selector: 'app-date-picker-range',
  templateUrl: './date-picker-range.component.html',
  styleUrls: ['./date-picker-range.component.scss'],
})
export class DatePickerRangeComponent implements OnInit {
  minStr: string | null = null;
  @Input() set minDate(value: Date | null | undefined) {
    if (value == undefined) {
      this.minStr = null;
    } else {
      this.minStr = this.datePipe.transform(value, 'yyyy-MM-dd')!;
    }
  }
  maxStr: string | null = null;
  @Input() set maxDate(value: Date | null | undefined) {
    if (value == undefined) {
      this.maxStr = null;
    } else {
      this.maxStr = this.datePipe.transform(value, 'yyyy-MM-dd')!;
    }
  }
  @Input()
  minDateLabel = 'Min Date:';
  @Input()
  maxDateLabel = 'Max Date:';
  @Input()
  class = '';
  @Input()
  disabled = false;
  @Input()
  readonly = false;

  @Output()
  minDateChange = new EventEmitter<Date | undefined>();
  @Output()
  maxDateChange = new EventEmitter<Date | undefined>();

  hoveredDate: NgbDate | null = null;

  constructor(
    private readonly calendar: NgbCalendar,
    private readonly datePipe: DatePipe
  ) {}

  ngOnInit(): void {}

  getToday(): NgbDate {
    return this.calendar.getToday();
  }

  toNgbDate(value: string | null): NgbDate | undefined {
    const m = moment(value, 'YYYY-MM-DD');
    if (m == null || !moment.isMoment(m) || !m.isValid()) {
      return undefined;
    }
    return new NgbDate(m.year(), m.month() + 1, m.date());
  }

  toDate(value: string | null): Date | undefined {
    const m = moment(value, 'YYYY-MM-DD');
    if (m == null || !moment.isMoment(m) || !m.isValid()) {
      return undefined;
    }

    return m.toDate();
  }

  toStr(value: NgbDate | null): string | null {
    if (value == null) {
      return null;
    }

    const d = new Date(value.year, value.month - 1, value.day);
    const dp = this.datePipe.transform(d, 'yyyy-MM-dd')!;
    return dp;
  }

  onDateSelection(date: NgbDate | null) {
    const minDate = this.toNgbDate(this.minStr);
    const maxDate = this.toNgbDate(this.maxStr);

    if (minDate == null || date?.before(minDate)) {
      this.minStr = this.toStr(date);
      this.minDateChange.emit(this.toDate(this.minStr));
    } else if (maxDate == null || date?.after(maxDate)) {
      this.maxStr = this.toStr(date);
      this.maxDateChange.emit(this.toDate(this.maxStr));
    } else {
      this.minStr = this.toStr(date);
      this.minDateChange.emit(this.toDate(this.minStr));
    }
  }

  isHovered(date: NgbDate) {
    return (
      this.minStr &&
      !this.maxStr &&
      this.hoveredDate &&
      date.after(this.toNgbDate(this.minStr)) &&
      date.before(this.hoveredDate)
    );
  }

  isInside(date: NgbDate) {
    return (
      this.maxStr &&
      date.after(this.toNgbDate(this.minStr)) &&
      date.before(this.toNgbDate(this.maxStr))
    );
  }

  isRange(date: NgbDate) {
    return (
      date.equals(this.toNgbDate(this.minStr)) ||
      (this.maxStr && date.equals(this.toNgbDate(this.maxStr))) ||
      this.isInside(date) ||
      this.isHovered(date)
    );
  }

  format(date: NgbDate | null): string | null {
    if (date == null) {
      return null;
    }

    const d = new Date(date.year, date.month - 1, date.day);
    const dp = this.datePipe.transform(d, 'yyyy-MM-dd')!;
    return dp;
  }

  onBlur(): void {
    if (this.minStr != null && this.maxStr != null) {
      const minDate = this.toNgbDate(this.minStr);
      const maxDate = this.toNgbDate(this.maxStr);

      if (maxDate!.before(minDate)) {
        this.minStr = this.toStr(maxDate!);
        this.maxStr = this.toStr(minDate!);
      }
    }
    this.minDateChange.emit(this.toDate(this.minStr));
    this.maxDateChange.emit(this.toDate(this.maxStr));
  }

  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const m = moment(input, 'YYYY-MM-DD');
    if (m == null || !moment.isMoment(m) || !m.isValid()) {
      return currentValue;
    }

    return new NgbDate(m.year(), m.month() + 1, m.date());
  }
}
