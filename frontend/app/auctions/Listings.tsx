"use client";

import React, { useEffect, useState } from "react";
import AuctionCard from "./AuctionCard";
import { Auction } from "../types/Auction";
import { AppPagination } from "../components/AppPagination";
import { getData } from "../actions/auctionActions";
import Filter from "./Filter";

export default function Listings() {
	const [pageNumber, setPageNumber] = useState(1);
	const [pageSize, setPageSize] = useState(4);
	const [pageCount, setPageCount] = useState(0);
	const [auctions, setAuctions] = useState<Auction[]>([]);

	useEffect(() => {
		getData(pageNumber, pageSize).then((data) => {
			setAuctions(data.results);
			setPageCount(data.pageCount);
		});
	}, [pageNumber, pageSize]);

	if (auctions.length === 0) return <span>Loading...</span>;
	return (
		<>
			<Filter pageSize={pageSize} setPageSize={setPageSize} />
			<div className="grid grid-cols-4 gap-6">
				{auctions.map((auction) => (
					<AuctionCard auction={auction} key={auction.id} />
				))}
			</div>
			<AppPagination
				currentPage={pageNumber}
				pageCount={pageCount}
				onPageChange={setPageNumber}
			/>
		</>
	);
}
