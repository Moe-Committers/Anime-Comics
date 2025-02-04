export function Comments() {
  return (
    <div className="space-y-4">
      <div className="bg-[#2a2a2a] rounded-lg p-4">
        <div className="flex items-start gap-3">
          <div className="w-10 h-10 rounded-full bg-gray-700" />
          <div className="flex-1">
            <textarea
              placeholder="Write a comment..."
              className="w-full px-3 py-2 bg-[#363636] text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500 resize-none"
              rows={3}
            />
            <button className="mt-2 px-4 py-1.5 bg-orange-500 hover:bg-orange-600 text-white rounded">
              Comment
            </button>
          </div>
        </div>
      </div>
      <div className="space-y-4">
        {Array(3)
          .fill(0)
          .map((_, i) => (
            <div key={i} className="bg-[#2a2a2a] rounded-lg p-4">
              <div className="flex gap-3">
                <div className="w-10 h-10 rounded-full bg-gray-700" />
                <div>
                  <div className="flex items-center gap-2">
                    <span className="font-medium text-white">User Name</span>
                    <span className="text-gray-400 text-sm">2 hours ago</span>
                  </div>
                  <p className="text-gray-300 mt-1">
                    Sample comment text here...
                  </p>
                </div>
              </div>
            </div>
          ))}
      </div>
    </div>
  );
}
