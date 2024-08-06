"use server";

import { Auction } from "../types/Auction";
import { PagedResult } from "../types/PagedResult";

export async function getData(
	pageNumber: number,
	pageSize: number
): Promise<PagedResult<Auction>> {
	const res = await fetch(
		`http://localhost:6001/search?pageNumber=${pageNumber}&pageSize=${pageSize}&orderBy=newest`
	);

	if (!res.ok) throw new Error("Failed to fetch data");

	return res.json();
}
