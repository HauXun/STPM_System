import React, { ReactNode, useEffect, useState } from 'react';

interface ErrorBoundaryProps {
  children: ReactNode;
}

function useErrorBoundary(): [boolean, React.Dispatch<React.SetStateAction<boolean>>] {
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    const handleOnError = () => {
      setHasError(true);
    };

    window.addEventListener('error', handleOnError);

    return () => {
      window.removeEventListener('error', handleOnError);
    };
  }, []);

  return [hasError, setHasError];
}

function ErrorBoundary({ children }: ErrorBoundaryProps) {
  const [hasError, setHasError] = useErrorBoundary();

  if (hasError) {
    // Fallback UI when an error occurs
    return <h1>Something went wrong.</h1>;
  }

  return <>{children}</>;
}

export default ErrorBoundary;
