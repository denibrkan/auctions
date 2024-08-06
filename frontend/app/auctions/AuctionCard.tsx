import React from "react";
import CountdownTimer from "./CountdownTimer";
import CarImage from "./CarImage";

type Props = {
	auction: any;
};

export default function AuctionCard({ auction }: Props) {
	return (
		<a className="bg-slate-100 rounded-lg shadow-md">
			<div className="group relative w-full bg-gray-200 aspect-video rounded-lg overflow-hidden">
				<CarImage imageUrl={auction.imageUrl} title={auction.title} />
				<div className="absolute bottom-2 left-2">
					<CountdownTimer dateEnd={auction.dateEnd} />
				</div>
			</div>
			<div className="px-3 py-2">
				<h5>{auction.title}</h5>
			</div>
		</a>
	);
}
