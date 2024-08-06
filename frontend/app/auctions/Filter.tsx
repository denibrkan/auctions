import { Button } from "flowbite-react";
import React from "react";

const pageSizeButtons: number[] = [4, 8, 12];

type Props = {
	pageSize: number;
	setPageSize: (pageSize: number) => void;
};

export default function Filter({ pageSize, setPageSize }: Props) {
	return (
		<div className="flex items-center mb-4">
			<span className="text-gray-500 text-sm mr-2 uppercase">Page size</span>
			<Button.Group>
				{pageSizeButtons.map((value, i) => (
					<Button
						key={i}
						color={`${pageSize == value ? "red" : "gray"}`}
						onClick={() => setPageSize(value)}
					>
						{value}
					</Button>
				))}
			</Button.Group>
		</div>
	);
}
