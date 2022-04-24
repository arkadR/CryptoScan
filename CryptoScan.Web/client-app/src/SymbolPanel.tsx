import {useState} from 'react';
import {CryptocurrencySymbol } from './Model/Symbol';
import {
  TextField,
  Button,
  TableRow,
  TableCell,
} from "@material-ui/core";

type SymbolPanelProps ={
  symbol: CryptocurrencySymbol;
  subscribeAction: (symbol: CryptocurrencySymbol, threshold: number) => void;
}

export default function SymbolPanel(props: SymbolPanelProps) {
  
  let [threshold, setThreshold] = useState<number | null>(null)

  let handleThresholdChange = (value: string) => {
    setThreshold(value as unknown as number);
  }

  return (
    <TableRow>
      <TableCell>{props.symbol.baseAsset}</TableCell>
      <TableCell>
        <TextField
          id={props.symbol.symbol}
          label="Threshold"
          type="number"
          InputLabelProps={{
            shrink: true,
          }}
          variant="outlined"
          onChange={(e) => handleThresholdChange(e.target.value)}
        />
        </TableCell>
        <TableCell>
          <Button 
            disabled={threshold === null}
            onClick={() => props.subscribeAction(props.symbol, threshold as number)}>
            Subscribe
          </Button>
        </TableCell>
    </TableRow>
  );
}