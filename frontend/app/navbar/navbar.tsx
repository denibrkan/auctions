import React from "react";
import { FaCommentDollar } from "react-icons/fa";

export default function navbar() {
	return (
		<header className="sticky top-0 z-50 flex justify-between bg-white p-5 items-center text-sky-600 shadow-md">
			<div className="flex items-center gap-2 text-3xl font-semibold text">
				<FaCommentDollar size={34} />
				<div>Auctions</div>
			</div>
			<div>search</div>
			<div>login</div>
		</header>
	);
}
