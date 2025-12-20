import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "BANGERFINDER [dashboard]",
  description: "Dashboard for BANGERFINDER application",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <div className="w-full min-h-screen bg-zinc-100 flex flex-col">
      {children}
    </div>
  );
}
