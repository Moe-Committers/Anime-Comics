import { UpdateItem } from "./UpdateItem";

export function UpdatesCard({ books, timeSince }) {
  return (
    <div className="bg-[#2a2a2a] rounded-lg overflow-hidden">
      {books.map((book) => (
        <UpdateItem key={book.id} book={book} timeSince={timeSince} />
      ))}
    </div>
  );
}
