import moment from 'moment/moment';

export class DateTimeValueConverter {
  toView(value, format) {
    if (!value) {
      return value;
    }

    return moment(value).format(format);
  }
}

export class ShortDateValueConverter {
  toView(value) {
    if (!value) {
      return value;
    }

    return moment(value).format("L");
  }
}
