//     This code was generated by a Reinforced.Typings tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.

import { HistoryDataIntervalEnum } from './model.enum';
import { LogLevelEnum } from './model.enum';

export interface IPaginate
{
	pageIndex: number;
	pageItemCount: number;
	totalPages: number;
	totalItemCount: number;
}
export interface IPaginateGeneric<T>
{
	items: T[];
	pageIndex: number;
	pageItemCount: number;
	totalPages: number;
	totalItemCount: number;
}
export interface IPaginateGenericFilter<T>
{
	filter: T;
	pageIndex: number;
	pageItemCount: number;
	orderBy?: string;
	isOrderAsc: boolean;
	isPaginated: boolean;
}
export interface IDropDown
{
	id: number;
	name: string;
}
export interface IPaginateFilter
{
	pageIndex: number;
	pageItemCount: number;
	orderBy?: string;
	isOrderAsc: boolean;
	isPaginated: boolean;
}
export interface IStringData
{
	data: string;
}
export interface IClientToken
{
	userId: number;
	userName: string;
	exp: Date;
	iat: Date;
	nbf: Date;
	roles: string[];
}
export interface ILogin
{
	userName: string;
	password: string;
}
export interface IHistoryData
{
	tickerId: number;
	interval: HistoryDataIntervalEnum;
	open: number;
	close: number;
	high: number;
	low: number;
	volume: number;
	dateTime: Date;
	id: number;
}
export interface ITicker
{
	symbol: string;
	id: number;
}
export interface ITickerFilter
{
	symbol?: string;
}
export interface ITickerInfo
{
	tickerId: number;
	name: string;
	quoteType: string;
	sector: string;
	industry: string;
	volume: number;
	isDelisted: boolean;
	id: number;
}
export interface IAppLog
{
	logLevel: LogLevelEnum;
	eventId: number;
	eventName?: string;
	source?: string;
	stackTrace?: string;
	message: string;
	createdDate: Date;
}
export interface IAppLogFilter
{
	logLevel?: LogLevelEnum;
	eventId?: number;
	eventName?: string;
	minDate?: Date;
	maxDate?: Date;
}
export interface ICronLog
{
	id: number;
	name: string;
	message?: string;
	isCanceled: boolean;
	startDate: Date;
	endDate: Date;
}
export interface ICronLogFilter
{
	name?: string;
	isCanceled?: boolean;
	minDate?: Date;
	maxDate?: Date;
}
export interface IPythonLog
{
	file: string;
	output?: string;
	error?: string;
	startDate: Date;
	endDate: Date;
}
export interface IPythonLogFilter
{
	file?: string;
	hasError?: boolean;
	minDate?: Date;
	maxDate?: Date;
}
export interface IAppUser
{
}
