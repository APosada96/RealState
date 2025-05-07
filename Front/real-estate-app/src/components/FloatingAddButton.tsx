'use client';

import Link from 'next/link';

export default function FloatingAddButton() {
  return (
    <Link
      href="/new"
      className="fixed bottom-6 right-6 bg-green-600 hover:bg-green-700 text-white px-6 py-3 rounded-full shadow-lg text-lg font-semibold transition-all"
    >
      + Add
    </Link>
  );
}