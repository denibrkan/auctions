"use client";

import React from "react";
import Countdown, { zeroPad } from "react-countdown";

type Props = {
	dateEnd: string;
};

// Renderer callback with condition
const renderer = ({
	days,
	hours,
	minutes,
	seconds,
	completed,
}: {
	days: number;
	hours: number;
	minutes: number;
	seconds: number;
	completed: boolean;
}) => {
	return (
		<div
			className={`border-2 border-white text-white text-sm rounded-lg px-2 py-1 opacity-70
            ${
							completed
								? "bg-red-600"
								: days < 1 && hours < 10
								? "bg-amber-600"
								: "bg-green-600"
						}
        `}
		>
			{completed ? (
				<span>Finished</span>
			) : days >= 1 ? (
				<span>
					{days} days {hours} hours
				</span>
			) : (
				<span>
					{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
				</span>
			)}
		</div>
	);
};

export default function CountdownTimer({ dateEnd }: Props) {
	return (
		<div>
			<Countdown renderer={renderer} date={dateEnd} />
		</div>
	);
}
