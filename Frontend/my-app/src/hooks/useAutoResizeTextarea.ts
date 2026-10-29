import { useCallback, useRef } from 'react';

export function useAutoResizeTextarea(maxHeight: number) {
  const ref = useRef<HTMLTextAreaElement | null>(null);

  const resize = useCallback(() => {
    const element = ref.current;
    if (!element) return;
    element.style.height = 'auto';
    element.style.height = `${Math.min(element.scrollHeight, maxHeight)}px`;
  }, [maxHeight]);

  return { ref, resize };
}
