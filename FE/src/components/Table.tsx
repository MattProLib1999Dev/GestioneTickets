import React, { useEffect, useState } from "react";
import { getAllAccount } from "../api/Account";

type Account = {
  email: string;
  nome: string;
  cognome: string;
  oreLavorate: number;
  ticket: number;
};

export function AccountsTable() {
  const [accounts, setAccounts] = useState<Account[]>([]);

  useEffect(() => {
    const fetchAccounts = async () => {
      try {
        const response = await getAllAccount();
        setAccounts(response.data as Account[]);
      } catch (error) {
        console.error("Errore nel recupero degli account:", error);
      }
    };

    fetchAccounts();
  }, []);

  return (
    <div>
      <h1>Tabella dei Dipendenti</h1>
      <table>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Cognome</th>
            <th>Ore Lavorate</th>
            <th>Ticket</th>
          </tr>
        </thead>
        <tbody>
          {accounts.map((acc) => (
            <tr key={acc.email}>
              <td>{acc.nome}</td>
              <td>{acc.cognome}</td>
              <td>{acc.oreLavorate}</td>
              <td>{acc.ticket}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

// Pagina che mostra la tabella
export default function TablePage() {
  return <AccountsTable />;
}
