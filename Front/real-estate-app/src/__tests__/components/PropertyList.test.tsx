import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import PropertyList from '../../components/PropertyList';
import { getProperties, deleteProperty } from '../../services/propertyService';
import { Property } from '../../types/property';
import { act } from 'react';

jest.mock('../../services/propertyService');

const mockProperties: Property[] = [
  {
    id: '1',
    name: 'Test Property',
    address: '123 Main St',
    price: 100000,
    imageUrl: '/images/test.png',
    idOwner: 'owner123',
  },
];

describe('PropertyList', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('renders property cards after loading', async () => {
    (getProperties as jest.Mock).mockResolvedValueOnce(mockProperties);

    await act(async () => {
      render(<PropertyList />);
    });

    expect(await screen.findByText('Test Property')).toBeInTheDocument();
    expect(screen.getByText('123 Main St')).toBeInTheDocument();
  });

  it('filters by name when input is used and submitted', async () => {
    (getProperties as jest.Mock).mockResolvedValue(mockProperties);

    await act(async () => {
      render(<PropertyList />);
    });

    const nameInput = screen.getByPlaceholderText('Name');
    fireEvent.change(nameInput, { target: { value: 'Test' } });

    const [filterBtn] = screen.getAllByRole('button', { name: /Filter/i });
    fireEvent.click(filterBtn);

    await waitFor(() => {
      expect(getProperties).toHaveBeenCalledWith({
        name: 'Test',
        address: undefined,
        minPrice: undefined,
        maxPrice: undefined,
      });
    });
  });

  it('clears filters and reloads all properties', async () => {
    (getProperties as jest.Mock).mockResolvedValue(mockProperties);

    await act(async () => {
      render(<PropertyList />);
    });

    const clearButton = screen.getByRole('button', { name: /Clear Filters/i });
    fireEvent.click(clearButton);

    await waitFor(() => {
      expect(getProperties).toHaveBeenCalledTimes(2);
    });
  });

 
});