/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { OrderDirectionEnum } from '../models/order-direction-enum';
import { SearchTagResponse } from '../models/search-tag-response';
export interface PageableResponseOfSearchTagResponse {
  data?: Array<SearchTagResponse>;
  orderBy?: string;
  orderDirection?: OrderDirectionEnum;
  pageCount?: number;
  pageNumber?: number;
  pageSize?: number;
  totalRecords?: number;
}
