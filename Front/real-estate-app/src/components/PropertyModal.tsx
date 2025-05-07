'use client';

import { Property } from '../types/property';

type Props = {
  property: Property | null;
  onClose: () => void;
};

export default function PropertyModal({ property, onClose }: Props) {
  if (!property) return null;

  return (
    <div className="fixed inset-0 backdrop-blur-sm bg-black/30 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-[90%] max-w-md">
        <h2 className="text-2xl font-bold mb-2">{property.name}</h2>
        <img
        src={property.imageUrl}
        alt={property.name}
        width={300}
        height={200}
        className="w-full h-auto rounded"
      />
      <br />
        <p className="text-black-700 font-bold">Address: {property.address}</p>
        <p className="text-black-700 font-bold">Owner: {property.idOwner}</p>
        <p className="text-green-700 font-bold">Price: ${property.price}</p>
        <div className="mt-4 flex justify-end">
          <button className="bg-gray-500 text-white px-4 py-2 rounded" onClick={onClose}>
            Close
          </button>
        </div>
      </div>
    </div>
  );
}