import '@testing-library/jest-dom';
import { render, screen } from '@testing-library/react';
import FloatingAddButton from '../../components/FloatingAddButton';
import { MemoryRouterProvider } from 'next-router-mock/MemoryRouterProvider';

describe('FloatingAddButton', () => {
  it('renders the button with correct text', () => {
    render(
      <MemoryRouterProvider>
        <FloatingAddButton />
      </MemoryRouterProvider>
    );

    const button = screen.getByRole('link', { name: /\+ add/i });
    expect(button).toBeInTheDocument();
  });

  it('has the correct href', () => {
    render(
      <MemoryRouterProvider>
        <FloatingAddButton />
      </MemoryRouterProvider>
    );

    const button = screen.getByRole('link', { name: /\+ add/i });
    expect(button).toHaveAttribute('href', '/new');
  });

  it('has correct styling classes', () => {
    render(
      <MemoryRouterProvider>
        <FloatingAddButton />
      </MemoryRouterProvider>
    );

    const button = screen.getByRole('link', { name: /\+ add/i });
    expect(button).toHaveClass('fixed', 'bottom-6', 'right-6', 'bg-green-600');
  });
});