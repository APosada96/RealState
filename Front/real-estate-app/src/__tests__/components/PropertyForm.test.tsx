import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import PropertyForm from '@/components/PropertyForm';
import { createProperty } from '@/services/propertyService';
import { useRouter } from 'next/navigation';
import { act } from 'react';
import '@testing-library/jest-dom';

jest.mock('@/services/propertyService');
jest.mock('next/navigation', () => ({
  useRouter: jest.fn(),
}));
jest.mock('react-hot-toast', () => ({
  success: jest.fn(),
  error: jest.fn(),
}));

describe('PropertyForm', () => {
  const pushMock = jest.fn();

  beforeEach(() => {
    (useRouter as jest.Mock).mockReturnValue({ push: pushMock });
    (createProperty as jest.Mock).mockResolvedValue({});
  });

  it('renders all form fields correctly', async () => {
    await act(async () => {
        render(<PropertyForm />);
      });
      await waitFor(() => {
        expect(screen.getByLabelText(/ID Owner/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Name/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Address/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Price/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Image/i)).toBeInTheDocument();
    });
  });

  it('shows validation messages if fields are empty', async () => {
    await act(async () => {
        render(<PropertyForm />);
      });

    fireEvent.click(screen.getByRole('button', { name: /save property/i }));

    await waitFor(() => {
      expect(screen.getByText(/Owner ID is required/i)).toBeInTheDocument();
      expect(screen.getByText(/The name is required/i)).toBeInTheDocument();
      expect(screen.getByText(/Address is mandatory/i)).toBeInTheDocument();
      expect(screen.getByText(/The price must be greater than 0|The price must be a number|Expected number/i )).toBeInTheDocument();
      expect(screen.getByText(/You must upload an image/i)).toBeInTheDocument();
    });
  });

  it('submits form when valid', async () => {
    await act(async () => {
        render(<PropertyForm />);
      });

    const file = new File(['image'], 'test.png', { type: 'image/png' });

    fireEvent.change(screen.getByLabelText(/ID Owner/i), {
      target: { value: 'owner123' },
    });
    fireEvent.change(screen.getByLabelText(/Name/i), {
      target: { value: 'Test House' },
    });
    fireEvent.change(screen.getByLabelText(/Address/i), {
      target: { value: '123 Main St' },
    });
    fireEvent.change(screen.getByLabelText(/Price/i), {
      target: { value: 100000 },
    });
    fireEvent.change(screen.getByLabelText(/Image/i), {
      target: { files: [file] },
    });

    fireEvent.click(screen.getByRole('button', { name: /save property/i }));

    await waitFor(() => {
      expect(createProperty).toHaveBeenCalledTimes(1);
      expect(pushMock).toHaveBeenCalledWith('/');
    });
  });
});