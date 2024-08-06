"use client";

import React, { useState } from "react";
import Image from "next/image";

type Props = {
	imageUrl: string;
	title: string;
};

export default function CarImage({ imageUrl, title }: Props) {
	const [loading, setLoading] = useState(true);

	return (
		<Image
			src={imageUrl}
			alt={title}
			fill
			priority
			className={`object-cover 
                group-hover:scale-110
                cursor-pointer
                duration-300
                ease-in-out
                ${loading ? "scale-110" : "blur-0 scale-100"}`}
			sizes="(max-width:768px) 100vw, (max-width:1200px) 50vw, 25vw"
			onLoad={() => setLoading(false)}
		/>
	);
}
