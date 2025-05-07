import "./globals.css";
import Navbar from './Navbar';
import { ReactNode } from 'react';
import { Toaster } from 'react-hot-toast';

export const metadata = {
  title: 'Real State Web',
  description: 'Real estate property management app',
};

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang="en">
      <body className="bg-gray-100 min-h-screen text-gray-800">
        <Navbar />
        <main className="p-4 max-w-6xl mx-auto">{children}</main>
        <Toaster position="top-right" />
      </body>
    </html>
  );
}