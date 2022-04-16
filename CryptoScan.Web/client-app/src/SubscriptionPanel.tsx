import {Subscription} from "./Model/Subscription";
import {useState} from 'react';
import {
  TextField,
  Button,
  TableRow,
  TableCell,
} from "@material-ui/core";

type SubscriptionPanelProps ={
  subscription: Subscription;
  updateAction: (subscription: Subscription, threshold: number) => void;
  deleteAction: (subscription: Subscription) => void;
}

export default function SubscriptionPanel(props: SubscriptionPanelProps){
  
  let [updatedThreshold, setUpdatedThreshold] = useState(props.subscription.threshold); 

  let handleThresholdChange = (value: string) => {
    setUpdatedThreshold(value as unknown as number);
  }

  return (
    <TableRow>
      <TableCell>
        {props.subscription.symbol.baseAsset}
      </TableCell>
      <TableCell>
        <TextField
          defaultValue={props.subscription.threshold}
          id={props.subscription.symbol.symbol}
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
          disabled={props.subscription.threshold === updatedThreshold}
          onClick={() => props.updateAction(props.subscription, updatedThreshold)}>
          Update
        </Button>
      </TableCell>
      <TableCell>
        <Button 
          onClick={() => props.deleteAction(props.subscription)}>
          Unsubscribe
        </Button>
      </TableCell>
    </TableRow>
  )
}