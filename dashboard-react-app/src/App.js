import React, { useEffect, useRef, useState } from 'react';
import './App.css';
import DashboardControl from 'devexpress-dashboard-react';
import { ResourceManager, loadDashboard, unloadDashboard } from 'devexpress-dashboard';
import { locale } from "devextreme/localization";
import datasource from "./data.json";

function switchMode(props) {
  return props === 'Viewer' ? "Designer" : "Viewer";
}

function switchLang(props) {
  return props === "pt-BR" ? "en-GB" : "pt-BR";
}

function loaddashbordo() {
  // loadDashboard("dashbordo").done(() => console.log("foi")).fail(() => console.log("nãofoi"))
  // dashboardControlGlobal.loadDashboard()
  console.log(unloadDashboard())
}

function App() {
  const [workingModeVar, setWorkingMode] = useState("Designer");
  const [lang, setLang] = useState("pt-BR");
  const [id, setId] = useState("dashboard10")
  const [IDs, setIDs] = useState()
  const [Dash, setDashboard] = useState("dashboard")
  const data = useRef()
  console.log(Dash)
  useEffect(() => {
    fetch("http://localhost:5000/api/dashboard/dashboards", { method: "GET" })
    .then(resp => resp.json())
    .then(res => setIDs(res))
  }, [id])
  useEffect(() => {
    if (Dash == "reloading")
      setDashboard("dashboard")
  }, [Dash])
  // Localize the Web Dashboard UI for the Spanish market (the 'es' culture):
  ResourceManager.setLocalizationMessages(require(`./DevExpressLocalizedResources_2021.2_${lang}/json resources/dx-dashboard.${lang}.json`));
  ResourceManager.setLocalizationMessages(require(`./DevExpressLocalizedResources_2021.2_${lang}/json resources/dx-analytics-core.${lang}.json`));
  // Apply culture-specific formatting:
  locale(lang);

  return (
    <div style={{ position: 'absolute', top: '0px', left: '0px', right: '0px', bottom: '0px' }}>
      <select onChange={(val) => setId(val.target.value)}>
        <option selected disabled>Dashboards</option>
        {IDs && IDs.map(value => (
          <option key={value.id} value={value.id} >{value.name}</option>
        ))
        }
      </select>
      <button onClick={() => { setWorkingMode(switchMode(workingModeVar)) }}>Switch to {switchMode(workingModeVar)}</button>
      <button onClick={() => { setLang(switchLang(lang)); setDashboard("reloading") }}>Switch to {switchLang(lang)}</button>
      {
        Dash === "dashboard" ? (
          <DashboardControl style={{ height: '100%' }} ref={data}
            endpoint={datasource.BaseURL}
            workingMode={workingModeVar}
            dashboardId={id}>
          </DashboardControl>)
          :
          (<div>Loading...</div>)
      }
    </div>
  );
}

export default App;