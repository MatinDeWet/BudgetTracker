/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { OrderDirectionEnum } from '../models/order-direction-enum';
import { SearchAccountResponse } from '../models/search-account-response';
export interface PageableResponseOfSearchAccountResponse {
  data?: Array<SearchAccountResponse>;
  orderBy?: string;
  orderDirection?: OrderDirectionEnum;
  pageCount?: number;
  pageNumber?: number;
  pageSize?: number;
  totalRecords?: number;
}
