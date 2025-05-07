import api from './api';
import { Property } from '../types/property';

export const getProperties = async (filters?: {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
}): Promise<Property[]> => {
  try {
    const params = new URLSearchParams();
    if (filters?.name) params.append('name', filters.name);
    if (filters?.address) params.append('address', filters.address);
    if (filters?.minPrice) params.append('minPrice', filters.minPrice.toString());
    if (filters?.maxPrice) params.append('maxPrice', filters.maxPrice.toString());

    const query = params.toString();
    const url = query ? `/properties?${query}` : `/properties`;

    const response = await api.get<Property[]>(url);
    return response.data;
  } catch (error) {
    console.error('Error al obtener propiedades:', error);
    throw error;
  }
};

export const getPropertyById = async (id: string): Promise<Property> => {
  const response = await api.get<Property>(`/properties/${id}`);
  return response.data;
};

export const createProperty = async (formData: FormData): Promise<Property> => {
    const response = await api.post<Property>(`/properties`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  };

  export const deleteProperty = async (id: string): Promise<void> => {
    await api.delete(`/properties/${id}`);
  };
  