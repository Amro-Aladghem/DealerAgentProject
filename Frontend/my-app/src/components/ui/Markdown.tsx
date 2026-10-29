import { useMemo } from 'react';
import { marked } from 'marked';
import { cn } from '@/libs/cn';

interface MarkdownProps {
  text: string;
  className?: string;
}

/**
 * Lightweight markdown renderer for agent messages.
 * Raw HTML is neutralised (only markdown output is rendered), RTL-safe via dir="auto",
 * single newlines become <br> and blank lines become paragraphs.
 */
export function Markdown({ text, className }: MarkdownProps) {
  const html = useMemo(() => {
    const safeText = text.replace(/</g, '&lt;');
    return marked.parse(safeText, {
      async: false,
      breaks: true,
      gfm: true,
    });
  }, [text]);

  return (
    <div
      dir="auto"
      className={cn('markdown', className)}
      dangerouslySetInnerHTML={{ __html: html }}
    />
  );
}
