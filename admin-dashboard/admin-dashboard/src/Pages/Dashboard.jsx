import { useEffect, useState } from 'react';
import API from '../api';

export default function Dashboard() {
    const [clients, setClients] = useState([]);
    const [rate, setRate] = useState(null);
    const [newRate, setNewRate] = useState('');
    const [payments, setPayments] = useState([]);

    useEffect(() => {
        API.get('/clients').then(res => setClients(res.data));
        API.get('/rate').then(res => {
            setRate(res.data?.currentRate ?? 0);
        });
        API.get('/payments?take=5').then(res => setPayments(res.data));
    }, []); 


    const updateRate = () => {
        API.post('/rate', parseFloat(newRate), {
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(() => {
            setRate(parseFloat(newRate));
            setNewRate('');
        });
    };


    return (
        <div>
            <h2>Clients</h2>

            <table style={{ marginTop: '0.5rem', borderCollapse: 'collapse', width: '100%' }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>ID</th>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>Name</th>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>Email</th>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>BalanceT</th>
                    </tr>
                </thead>
                <tbody>
                    {clients.map(c => (
                        <tr key={c.id}>
                            <td style={{ padding: '4px' }}>{c.id}</td>
                            <td style={{ padding: '4px' }}>{c.name}</td>
                            <td style={{ padding: '4px' }}>{c.email}</td>
                            <td style={{ padding: '4px' }}>{c.balanceT} tokens</td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <h3 className="text-lg font-semibold mt-6"><br></br><br></br>Last payments</h3>
            <table style={{ marginTop: '0.5rem', borderCollapse: 'collapse', width: '100%' }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>Client</th>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>Amount</th>
                        <th style={{ textAlign: 'left', borderBottom: '1px solid #ccc', padding: '4px' }}>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {payments.map(p => (
                        <tr key={p.id}>
                            <td style={{ padding: '4px' }}>{p.client}</td>
                            <td style={{ padding: '4px' }}>{p.amountT} tokens</td>
                            <td style={{ padding: '4px' }}>{new Date(p.createdAt).toLocaleDateString()}</td>
                        </tr>
                    ))}
                </tbody>
            </table>


            <h2>Rate</h2>
            <p>Current rate: {rate}</p>
            <input value={newRate} onChange={(e) => setNewRate(e.target.value)} />
            <button onClick={updateRate}>Update</button>
        </div>
    );
}
