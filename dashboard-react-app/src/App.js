import React, { useRef } from 'react';
import './App.css';
import DashboardControl from 'devexpress-dashboard-react';

function App() {
  const data = useRef()
  console.log(data)
  return (
    <div style={{ position: 'absolute', top: '0px', left: '0px', right: '0px', bottom: '0px' }}>
      <DashboardControl style={{ height: '100%' }} ref={data}
        endpoint="http://localhost:5000/api/dashboard">
      </DashboardControl>
    </div>
  );
}

export default App;