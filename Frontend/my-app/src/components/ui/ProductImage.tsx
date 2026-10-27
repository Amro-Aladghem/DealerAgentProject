import { useState } from 'react';
import type { ImgHTMLAttributes } from 'react';
import { ImageOff } from 'lucide-react';
import { cn } from '@/libs/cn';

type ProductImageProps = Omit<ImgHTMLAttributes<HTMLImageElement>, 'src'> & {
  src?: string | null;
  fallbackClassName?: string;
};

export function ProductImage({ src, alt, className, fallbackClassName, ...props }: ProductImageProps) {
  const [failed, setFailed] = useState(!src);

  if (failed) {
    return (
      <div
        className={cn(
          'flex items-center justify-center bg-primary-soft text-primary/50',
          fallbackClassName ?? className,
        )}
      >
        <ImageOff size={28} strokeWidth={1.5} />
      </div>
    );
  }

  return (
    <img
      src={src ?? undefined}
      alt={alt}
      loading="lazy"
      onError={() => setFailed(true)}
      className={className}
      {...props}
    />
  );
}
