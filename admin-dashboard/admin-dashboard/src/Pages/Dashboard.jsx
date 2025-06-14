import { useEffect, useState } from 'react';
import API from '../api';

export default function Dashboard() {
    const [clients, setClients] = useState([]);
    const [payments, setPayments] = useState([]);
    const [editingClient, setEditingClient] = useState(null); // null = form closed
    const [formData, setFormData] = useState({ name: '', email: '', balanceT: '', tags: '' });
    const [formVisible, setFormVisible] = useState(false); // control form visibility

    useEffect(() => {
        fetchClients();
        fetchPayments();
    }, []);

    const fetchClients = () => {
        API.get('/clients').then(res => setClients(res.data));
    };

    const fetchPayments = () => {
        API.get('/payments?take=5').then(res => setPayments(res.data));
    };

    // Handle input changes in form
    const onChange = (e) => {
        setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    // Open form to edit existing client
    const onEdit = (client) => {
        setEditingClient(client);
        setFormData({
            name: client.name || '',
            email: client.email || '',
            balanceT: client.balanceT?.toString() || '',
            tags: client.tags || '',
        });
        setFormVisible(true);
    };

    // Open form to create new client
    const onCreate = () => {
        setEditingClient(null);
        setFormData({ name: '', email: '', balanceT: '', tags: '' });
        setFormVisible(true);
    };

    // Save client (update or create)
    const onSave = () => {
        const payload = {
            ...formData,
            balanceT: parseFloat(formData.balanceT) || 0,
        };

        if (editingClient) {
            API.put(`/clients/${editingClient.id}`, payload).then(() => {
                fetchClients();
                setFormVisible(false);
                setEditingClient(null);
            });
        } else {
            API.post('/clients', payload).then(() => {
                fetchClients();
                setFormVisible(false);
            });
        }
    };

    // Cancel form editing/creating
    const onCancel = () => {
        setFormVisible(false);
        setEditingClient(null);
        setFormData({ name: '', email: '', balanceT: '', tags: '' });
    };

    // Delete client by id
    const onDelete = (id) => {
        if (window.confirm('Delete client?')) {
            API.delete(`/clients/${id}`).then(() => fetchClients());
        }
    };

    return (
        <div>
            <h2>Clients</h2>
            <button onClick={onCreate}>Add Client</button>

            <table
                style={{
                    marginTop: '0.5rem',
                    borderCollapse: 'collapse',
                    width: '100%',
                    textAlign: 'left',
                }}
            >
                <thead>
                    <tr>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>ID</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Name</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Email</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>BalanceT</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Tags</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {clients.map((c) => (
                        <tr key={c.id}>
                            <td style={{ padding: '4px' }}>{c.id}</td>
                            <td style={{ padding: '4px' }}>{c.name}</td>
                            <td style={{ padding: '4px' }}>{c.email}</td>
                            <td style={{ padding: '4px' }}>{c.balanceT} tokens</td>
                            <td style={{ padding: '4px' }}>{c.tags}</td>
                            <td style={{ padding: '4px' }}>
                                <button onClick={() => onEdit(c)}>Edit</button>{' '}
                                <button onClick={() => onDelete(c.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {formVisible && (
                <div
                    style={{
                        marginTop: '1rem',
                        border: '1px solid #ccc',
                        padding: '1rem',
                        maxWidth: '400px',
                    }}
                >
                    <h3>{editingClient ? 'Edit Client' : 'New Client'}</h3>
                    <input
                        name="name"
                        placeholder="Name"
                        value={formData.name}
                        onChange={onChange}
                        style={{ display: 'block', marginBottom: '0.5rem', width: '100%' }}
                    />
                    <input
                        name="email"
                        placeholder="Email"
                        value={formData.email}
                        onChange={onChange}
                        style={{ display: 'block', marginBottom: '0.5rem', width: '100%' }}
                    />
                    <input
                        name="balanceT"
                        placeholder="BalanceT"
                        type="number"
                        value={formData.balanceT}
                        onChange={onChange}
                        style={{ display: 'block', marginBottom: '0.5rem', width: '100%' }}
                    />
                    <input
                        name="tags"
                        placeholder="Tags (comma separated)"
                        value={formData.tags}
                        onChange={onChange}
                        style={{ display: 'block', marginBottom: '0.5rem', width: '100%' }}
                    />
                    <button onClick={onSave} style={{ marginRight: '0.5rem' }}>
                        Save
                    </button>
                    <button onClick={onCancel}>Cancel</button>
                </div>
            )}

            <h3>Last payments</h3>
            <table
                style={{
                    marginTop: '0.5rem',
                    borderCollapse: 'collapse',
                    width: '100%',
                    textAlign: 'left',
                }}
            >
                <thead>
                    <tr>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Client</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Amount</th>
                        <th style={{ borderBottom: '1px solid #ccc', padding: '4px' }}>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {payments && payments.length > 0 ? (
                        payments.map((p) => (
                            <tr key={p.id}>
                                <td style={{ padding: '4px' }}>{p.client}</td>
                                <td style={{ padding: '4px' }}>{p.amountT} tokens</td>
                                <td style={{ padding: '4px' }}>{new Date(p.createdAt).toLocaleDateString()}</td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="3" style={{ padding: '4px', textAlign: 'center' }}>
                                No payments found
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}
