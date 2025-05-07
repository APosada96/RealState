'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useRouter } from 'next/navigation';
import { createProperty } from '../services/propertyService';
import { useState } from 'react';
import toast from 'react-hot-toast';

const schema = z.object({
  idOwner: z.string().min(1, 'Owner ID is required'),
  name: z.string().min(1, 'The name is required'),
  address: z.string().min(1, 'Address is mandatory'),
  price: z
  .number({
    required_error: 'The price is required',
    invalid_type_error: 'The price must be a number',
  })
  .min(1, 'The price must be greater than 0'),
  image: z.any().refine((file) => file?.length === 1, 'You must upload an image'),
});

type FormData = z.infer<typeof schema>;

export default function PropertyForm() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    try {
      setLoading(true);
      const formData = new FormData();
      formData.append('idOwner', data.idOwner);
      formData.append('name', data.name);
      formData.append('address', data.address);
      formData.append('price', data.price.toString());
      formData.append('image', data.image[0]);

      await createProperty(formData);
      toast.success('Correctly registered property!');

      reset();
      router.push('/');
    } catch (error: any) {
        if (error.response?.status === 409) {
            toast.error(error.response.data?.message || 'A property with that name and address already exists.');
        } else {
      toast.error('There was an error saving the property');
    }
} finally {
  setLoading(false);
}
};

  return (
    <div className="max-w-xl mx-auto">
      <div className="mb-6 flex justify-between items-center">
        <h2 className="text-2xl font-bold">Register Property</h2>
        <button
          type="button"
          onClick={() => router.push('/')}
          className="bg-gray-200 text-gray-700 px-4 py-2 rounded hover:bg-gray-300"
        >
          ⬅ Back
        </button>
      </div>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
        <label htmlFor="idOwner" className="block mb-1 font-medium">ID Owner</label>          
        <input id="idOwner"
            {...register('idOwner')}
            className={`border px-3 py-2 rounded w-full ${
              errors.idOwner ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.idOwner && (
            <p className="text-red-500 text-sm mt-1">{errors.idOwner.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="name" className="block mb-1 font-medium">Name</label>
          <input id="name"
            {...register('name')}
            className={`border px-3 py-2 rounded w-full ${
              errors.name ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.name && (
            <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="address" className="block mb-1 font-medium">Address</label>
          <input id="address"
            {...register('address')}
            className={`border px-3 py-2 rounded w-full ${
              errors.address ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.address && (
            <p className="text-red-500 text-sm mt-1">{errors.address.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="number" className="block mb-1 font-medium">Price</label>
          <input id="number"
            type="number"
            {...register('price', { valueAsNumber: true })}
            className={`border px-3 py-2 rounded w-full ${
              errors.price ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.price && (
            <p className="text-red-500 text-sm mt-1">{errors.price.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="image" className="block mb-1 font-medium">Image</label>
          <input id="image"
            type="file"
            accept="image/*"
            {...register('image')}
            className={`block w-full text-sm text-gray-900 border border-gray-300 rounded cursor-pointer bg-gray-50 ${
              errors.image ? 'border-red-500' : ''
            }`}
          />
          {errors.image && (
            <p className="text-red-500 text-sm mt-1">{errors.image.message as string}</p>
          )}
        </div>

        <button
          type="submit"
          disabled={loading}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          {loading ? 'Saving...' : 'Save Property'}
        </button>
      </form>
    </div>
  );
}
