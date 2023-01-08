import {
  IPaginateGeneric,
  IPaginateFilter,
  IPaginateGenericFilter,
} from './model';

export class ModelHelper {
  static IPaginateGenericDefault<T>(): IPaginateGeneric<T> {
    return {
      pageIndex: 0,
      pageItemCount: 0,
      totalPages: 0,
      totalItemCount: 0,
      items: [] as T[],
    } as IPaginateGeneric<T>;
  }

  static IPaginateFilterDefault(): IPaginateFilter {
    return {
      pageIndex: 0,
      pageItemCount: 20,
      orderBy: undefined,
      isOrderAsc: true,
      isPaginated: true,
    } as IPaginateFilter;
  }

  static IPaginateGenericFilterDefault(): IPaginateGenericFilter<unknown> {
    return {
      filter: {},
      pageIndex: 0,
      pageItemCount: 20,
      orderBy: undefined,
      isOrderAsc: true,
      isPaginated: true,
    } as IPaginateGenericFilter<unknown>;
  }
}
