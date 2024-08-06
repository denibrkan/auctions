/** @type {import('next').NextConfig} */
const nextConfig = {
	logging: {
		fetches: {
			fullUrl: true,
		},
	},
	images: {
		remotePatterns: [
			{
				hostname: "cdn.pixabay.com",
			},
		],
	},
};
export default nextConfig;
