//     This code was generated by a Reinforced.Typings tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.

import { HistoryDataIntervalEnum } from './model.enum';
import { DelistEnum } from './model.enum';
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
export interface INewsArticle
{
	title: string;
	link: string;
	text: string;
	date: Date;
	id: number;
}
export interface INewsFilter
{
	symbol?: string;
	minDate?: Date;
	maxDate?: Date;
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
	date: Date;
	id: number;
}
export interface IMarketSchedule
{
	day: Date;
	marketOpen: Date;
	marketClose: Date;
	id: number;
}
export interface ITickerDelistReason
{
	tickerId: number;
	delist: DelistEnum;
	createdDate: Date;
	id: number;
}
export interface ITicker
{
	symbol: string;
	lastHistoryUpdate?: Date;
	lastInfoUpdate?: Date;
	lastNewsUpdate?: Date;
	name: string;
	quoteType: string;
	exchange: string;
	marketCap?: number;
	sector?: string;
	industry?: string;
	volume?: number;
	payoutRatio?: number;
	dividendRate?: number;
	delistReasons: ITickerDelistReason[];
	id: number;
}
export interface ITickerFilter
{
	name?: string;
	symbol?: string;
	quoteType?: string;
	sector?: string;
	industry?: string;
	isListed?: boolean;
}
export interface ITickerHistorySummary
{
	valueSummaries: IValueSummary[];
}
export interface IValueSummary
{
	title: string;
	date?: Date;
	value?: number;
	difference?: number;
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
export interface ICronJobDetail
{
	name: string;
	cronExpression: string;
	nextOccurrence?: Date;
	lastOccurrence?: Date;
	isRunning: boolean;
}
export interface IAppUser
{
}
export interface IToken
{
	accessToken: string;
	refreshToken: string;
}
