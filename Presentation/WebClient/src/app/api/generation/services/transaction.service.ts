/* tslint:disable */
/* eslint-disable */
/* Code generated by ng-openapi-gen DO NOT EDIT. */

import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { createTransaction } from '../fn/transaction/create-transaction';
import { CreateTransaction$Params } from '../fn/transaction/create-transaction';
import { CreateTransactionResponse } from '../models/create-transaction-response';
import { deleteTransaction } from '../fn/transaction/delete-transaction';
import { DeleteTransaction$Params } from '../fn/transaction/delete-transaction';
import { getTransactionById } from '../fn/transaction/get-transaction-by-id';
import { GetTransactionById$Params } from '../fn/transaction/get-transaction-by-id';
import { GetTransactionByIdResponse } from '../models/get-transaction-by-id-response';
import { PageableResponseOfSeachTransactionResponse } from '../models/pageable-response-of-seach-transaction-response';
import { searchTransaction } from '../fn/transaction/search-transaction';
import { SearchTransaction$Params } from '../fn/transaction/search-transaction';
import { tagTransaction } from '../fn/transaction/tag-transaction';
import { TagTransaction$Params } from '../fn/transaction/tag-transaction';
import { untagTransaction } from '../fn/transaction/untag-transaction';
import { UntagTransaction$Params } from '../fn/transaction/untag-transaction';
import { updateTransaction } from '../fn/transaction/update-transaction';
import { UpdateTransaction$Params } from '../fn/transaction/update-transaction';

@Injectable({ providedIn: 'root' })
export class TransactionService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `updateTransaction()` */
  static readonly UpdateTransactionPath = '/transaction/update';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateTransaction()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateTransaction$Response(params: UpdateTransaction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateTransaction(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateTransaction$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  updateTransaction(params: UpdateTransaction$Params, context?: HttpContext): Observable<void> {
    return this.updateTransaction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `untagTransaction()` */
  static readonly UntagTransactionPath = '/transaction/untag';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `untagTransaction()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  untagTransaction$Response(params: UntagTransaction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return untagTransaction(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `untagTransaction$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  untagTransaction(params: UntagTransaction$Params, context?: HttpContext): Observable<void> {
    return this.untagTransaction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `tagTransaction()` */
  static readonly TagTransactionPath = '/transaction/tag';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `tagTransaction()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  tagTransaction$Response(params: TagTransaction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return tagTransaction(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `tagTransaction$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  tagTransaction(params: TagTransaction$Params, context?: HttpContext): Observable<void> {
    return this.tagTransaction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `searchTransaction()` */
  static readonly SearchTransactionPath = '/transaction/search';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `searchTransaction()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  searchTransaction$Response(params: SearchTransaction$Params, context?: HttpContext): Observable<StrictHttpResponse<PageableResponseOfSeachTransactionResponse>> {
    return searchTransaction(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `searchTransaction$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  searchTransaction(params: SearchTransaction$Params, context?: HttpContext): Observable<PageableResponseOfSeachTransactionResponse> {
    return this.searchTransaction$Response(params, context).pipe(
      map((r: StrictHttpResponse<PageableResponseOfSeachTransactionResponse>): PageableResponseOfSeachTransactionResponse => r.body)
    );
  }

  /** Path part for operation `getTransactionById()` */
  static readonly GetTransactionByIdPath = '/transaction/{id}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getTransactionById()` instead.
   *
   * This method doesn't expect any request body.
   */
  getTransactionById$Response(params: GetTransactionById$Params, context?: HttpContext): Observable<StrictHttpResponse<GetTransactionByIdResponse>> {
    return getTransactionById(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getTransactionById$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getTransactionById(params: GetTransactionById$Params, context?: HttpContext): Observable<GetTransactionByIdResponse> {
    return this.getTransactionById$Response(params, context).pipe(
      map((r: StrictHttpResponse<GetTransactionByIdResponse>): GetTransactionByIdResponse => r.body)
    );
  }

  /** Path part for operation `deleteTransaction()` */
  static readonly DeleteTransactionPath = '/transaction/{id}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `deleteTransaction()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteTransaction$Response(params: DeleteTransaction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return deleteTransaction(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `deleteTransaction$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteTransaction(params: DeleteTransaction$Params, context?: HttpContext): Observable<void> {
    return this.deleteTransaction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `createTransaction()` */
  static readonly CreateTransactionPath = '/transaction/create';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `createTransaction()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  createTransaction$Response(params: CreateTransaction$Params, context?: HttpContext): Observable<StrictHttpResponse<CreateTransactionResponse>> {
    return createTransaction(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `createTransaction$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  createTransaction(params: CreateTransaction$Params, context?: HttpContext): Observable<CreateTransactionResponse> {
    return this.createTransaction$Response(params, context).pipe(
      map((r: StrictHttpResponse<CreateTransactionResponse>): CreateTransactionResponse => r.body)
    );
  }

}
