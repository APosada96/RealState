'use client';

import { Property } from '../types/property';

type Props = {
  property: Property;
  onView: () => void;
  onDelete: () => void;
  isDeleting?: boolean;
};

export default function PropertyCard({ property, onView, onDelete, isDeleting = false }: Props) {
  return (
    <div className="bg-white rounded-lg shadow-md p-4">
      <img
        src={property.imageUrl}
        alt={property.name}
        className="rounded w-full h-48 object-cover"
      />
      <h3 className="text-xl font-semibold mt-2">{property.name}</h3>
      <p>{property.address}</p>
    
      <div className="flex gap-2 mt-4">
        <button className="bg-blue-500 text-white px-3 py-1 rounded" onClick={onView}>
          View Detail
        </button>

        {isDeleting ? (
          <div className="px-3 py-1 bg-gray-200 text-gray-500 rounded animate-pulse">
            Eliminating...
          </div>
        ) : (
          <button className="bg-red-500 text-white px-3 py-1 rounded" onClick={onDelete}>
            Remove
          </button>
        )}
      </div>
    </div>
  );
}
