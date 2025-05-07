import '@testing-library/jest-dom';
import { render, screen, fireEvent } from '@testing-library/react';
import PropertyModal from '../../components/PropertyModal';
import { Property } from '../../types/property';

describe('PropertyModal', () => {
  const mockProperty: Property = {
    id: '1',
    name: 'Test Property',
    address: '123 Test St',
    idOwner: 'owner-1',
    price: 500000,
    imageUrl: '/images/test.jpg'
  };

  it('does not render when property is null', () => {
    const { container } = render(<PropertyModal property={null} onClose={jest.fn()} />);
    expect(container.firstChild).toBeNull();
  });

  it('renders property details when provided', () => {
    render(<PropertyModal property={mockProperty} onClose={jest.fn()} />);

    expect(screen.getByText(/Test Property/i)).toBeInTheDocument();
    expect(screen.getByText(/123 Test St/i)).toBeInTheDocument();
    expect(screen.getByText(/Owner: owner-1/i)).toBeInTheDocument();
    expect(screen.getByText(/\$500000/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /close/i })).toBeInTheDocument();
    expect(screen.getByRole('img')).toHaveAttribute('src', mockProperty.imageUrl);
  });

  it('calls onClose when the close button is clicked', () => {
    const handleClose = jest.fn();
    render(<PropertyModal property={mockProperty} onClose={handleClose} />);

    const closeButton = screen.getByRole('button', { name: /close/i });
    fireEvent.click(closeButton);

    expect(handleClose).toHaveBeenCalledTimes(1);
  });
});