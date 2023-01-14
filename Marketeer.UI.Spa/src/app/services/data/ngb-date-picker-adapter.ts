import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

@Injectable()
export class NgbDatePickerAdapter extends NgbDateAdapter<string> {
  constructor(private readonly datePipe: DatePipe) {
    super();
  }

  fromModel(date: string | null): NgbDateStruct | null {
    const m = moment(date, 'YYYY-MM-DD');
    if (m == null || !moment.isMoment(m) || !m.isValid()) {
      return null;
    }

    return {
      year: m.year(),
      month: m.month() + 1,
      day: m.date(),
    };
  }

  toModel(date: NgbDateStruct | null): string | null {
    if (date == null) {
      return null;
    }

    const d = new Date(date.year, date.month - 1, date.day);
    const dp = this.datePipe.transform(d, 'yyyy-MM-dd')!;
    return dp;
  }
}
