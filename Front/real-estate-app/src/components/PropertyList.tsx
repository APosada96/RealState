'use client';

import { useEffect, useState } from 'react';
import { Property } from '../types/property';
import { getProperties, deleteProperty } from '../services/propertyService';
import PropertyCard from './PropertyCard';
import PropertyModal from './PropertyModal';
import toast from 'react-hot-toast';

export default function PropertyList() {
  const [properties, setProperties] = useState<Property[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [selected, setSelected] = useState<Property | null>(null);
  const [deletingId, setDeletingId] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 6;

  const [filters, setFilters] = useState({
    name: '',
    address: '',
    minPrice: '',
    maxPrice: ''
  });

  const indexOfLast = currentPage * itemsPerPage;
  const indexOfFirst = indexOfLast - itemsPerPage;
  const currentProperties = properties.slice(indexOfFirst, indexOfLast);
  const totalPages = Math.ceil(properties.length / itemsPerPage);

  useEffect(() => {
    loadProperties();
  }, [filters]);

  const loadProperties = async () => {
    try {
      setLoading(true);
      const data = await getProperties({
        name: filters.name || undefined,
        address: filters.address || undefined,
        minPrice: filters.minPrice ? Number(filters.minPrice) : undefined,
        maxPrice: filters.maxPrice ? Number(filters.maxPrice) : undefined,
      });
      setProperties(data);
    } catch (err) {
      setError('There was an error loading the properties.');
    } finally {
      setLoading(false);
    }
  };

  const showDeleteConfirmationToast = (onConfirm: () => void) => {
    toast.custom((t) => (
      <div className="bg-white rounded shadow-md p-4 w-72 border border-gray-200">
        <p className="text-sm text-gray-800 mb-3">Delete this property?</p>
        <div className="flex justify-end gap-2">
          <button
            className="text-gray-600 hover:text-gray-800 text-sm"
            onClick={() => toast.dismiss(t.id)}
          >
            Cancel
          </button>
          <button
            className="bg-red-600 text-white px-3 py-1 rounded text-sm hover:bg-red-700"
            onClick={() => {
              toast.dismiss(t.id);
              onConfirm();
            }}
          >
            Eliminate
          </button>
        </div>
      </div>
    ));
  };

  const handleDelete = (id: string) => {
    showDeleteConfirmationToast(async () => {
      try {
        setDeletingId(id);
        await deleteProperty(id);
        toast.success('Property successfully deleted.');
        await loadProperties();
      } catch {
        toast.error('Error deleting property.');
      } finally {
        setDeletingId(null);
      }
    });
  };

  const changePage = (page: number) => {
    if (page >= 1 && page <= totalPages) {
      setCurrentPage(page);
    }
  };

  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          setCurrentPage(1);
          
          loadProperties();
        }}
        className="mb-6 grid grid-cols-1 md:grid-cols-4 gap-4"
      >
        <input
          type="text"
          placeholder="Name"
          className="border px-3 py-2 rounded"
          value={filters.name}
          onChange={(e) => setFilters({ ...filters, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Address"
          className="border px-3 py-2 rounded"
          value={filters.address}
          onChange={(e) => setFilters({ ...filters, address: e.target.value })}
        />
        <input
          type="number"
          placeholder="Minimum Price"
          className="border px-3 py-2 rounded"
          value={filters.minPrice}
          onChange={(e) => setFilters({ ...filters, minPrice: e.target.value })}
        />
        <input
          type="number"
          placeholder="Maximum Price"
          className="border px-3 py-2 rounded"
          value={filters.maxPrice}
          onChange={(e) => setFilters({ ...filters, maxPrice: e.target.value })}
        />
     <div className="md:col-span-4 flex justify-center gap-4 mt-2">
        <button
            type="submit"
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
            Filter
        </button>

        <button
            type="button"
            className="bg-gray-300 text-gray-800 px-4 py-2 rounded hover:bg-gray-400"
            onClick={() => {
                setFilters({
                  name: '',
                  address: '',
                  minPrice: '',
                  maxPrice: ''
                });
                setCurrentPage(1);
              }}
        >
            Clear Filters
        </button>
    </div>
      </form>

      {loading &&  (
        <p className="text-center py-6 text-gray-600">Loading properties...</p>
      )}

      {error   && (
        <p className="text-center py-6 text-red-600">{error}</p>
      )}

      {!loading  && properties.length === 0 && (
        <p className="text-center py-6 text-gray-600">No properties found.</p>
      )}

      {!loading && properties.length > 0 && (
        <>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
            {currentProperties.map((property) => (
              <PropertyCard
                key={property.id}
                property={property}
                onView={() => setSelected(property)}
                onDelete={() => handleDelete(property.id)}
                isDeleting={deletingId === property.id}
              />
            ))}
          </div>

          <div className="flex justify-center mt-6 gap-2 flex-wrap">
            {Array.from({ length: totalPages }, (_, i) => i + 1).map((num) => (
              <button
                key={num}
                onClick={() => changePage(num)}
                className={`px-4 py-1 border rounded ${
                  currentPage === num
                    ? 'bg-blue-600 text-white'
                    : 'bg-white text-blue-600 border-blue-600'
                }`}
              >
                {num}
              </button>
            ))}
          </div>
        </>
      )}

      <PropertyModal property={selected} onClose={() => setSelected(null)} />
    </>
  );
}
