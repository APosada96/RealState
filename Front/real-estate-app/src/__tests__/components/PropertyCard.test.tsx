import { render, screen, fireEvent } from '@testing-library/react';
import PropertyCard from '@/components/PropertyCard';
import { Property } from '@/types/property';
import '@testing-library/jest-dom';

describe('PropertyCard', () => {
  const mockProperty: Property = {
    id: '1',
    idOwner: 'owner1',
    name: 'Modern Villa',
    address: 'Sunset Blvd',
    price: 200000,
    imageUrl: '/images/test.png',
  };

  it('renders property details', () => {
    render(
      <PropertyCard
        property={mockProperty}
        onView={() => {}}
        onDelete={() => {}}
      />
    );

    expect(screen.getByText(/Modern Villa/i)).toBeInTheDocument();
    expect(screen.getByText(/Sunset Blvd/i)).toBeInTheDocument();
    expect(screen.getByRole('img')).toHaveAttribute('src', '/images/test.png');
  });

  it('calls onView when "View Detail" is clicked', () => {
    const onViewMock = jest.fn();
    render(
      <PropertyCard
        property={mockProperty}
        onView={onViewMock}
        onDelete={() => {}}
      />
    );

    fireEvent.click(screen.getByText(/View Detail/i));
    expect(onViewMock).toHaveBeenCalled();
  });

  it('calls onDelete when "Remove" is clicked', () => {
    const onDeleteMock = jest.fn();
    render(
      <PropertyCard
        property={mockProperty}
        onView={() => {}}
        onDelete={onDeleteMock}
      />
    );

    fireEvent.click(screen.getByText(/Remove/i));
    expect(onDeleteMock).toHaveBeenCalled();
  });

  it('shows "Eliminating..." when isDeleting is true', () => {
    render(
      <PropertyCard
        property={mockProperty}
        onView={() => {}}
        onDelete={() => {}}
        isDeleting={true}
      />
    );

    expect(screen.getByText(/Eliminating/i)).toBeInTheDocument();
    expect(screen.queryByText(/Remove/i)).not.toBeInTheDocument();
  });
});