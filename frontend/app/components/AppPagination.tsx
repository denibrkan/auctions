"use client";

import { Pagination } from "flowbite-react";

type Props = {
	currentPage: number;
	pageCount: number;
	onPageChange: (pageNumber: number) => void;
};

export function AppPagination({ currentPage, pageCount, onPageChange }: Props) {
	return (
		<div className="flex overflow-x-auto sm:justify-center">
			<Pagination
				currentPage={currentPage}
				totalPages={pageCount}
				onPageChange={onPageChange}
			/>
		</div>
	);
}
