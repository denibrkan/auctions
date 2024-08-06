export type Auction = {
	reservePrice: number;
	seller: string;
	winner?: string;
	soldAmount?: number;
	currentHighBid: number;
	status: string;
	dateStart: string;
	dateEnd: string;
	title: string;
	description: string;
	imageUrl: string;
	category: string;
	dateCreated: string;
	dateUpdated: string;
	id: string;
};
