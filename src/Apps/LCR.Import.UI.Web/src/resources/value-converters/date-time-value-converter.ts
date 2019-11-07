import moment from 'moment';

export class DateTimeValueConverter {
  toView(value, format) {
    if (!value) {
      return value;
    }

    return moment(value).format(format);
  }
}
