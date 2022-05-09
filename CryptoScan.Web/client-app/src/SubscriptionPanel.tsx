import {Subscription, TimeRange, Trend} from "./Models";
import {useState} from 'react';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider, DatePicker } from '@mui/x-date-pickers';
import { 
  TextField as MuiTextField, 
  MenuItem,
  FormControl,
  Select,
  InputLabel } from '@mui/material';
import {
  TextField,
  Button,
  TableRow,
  TableCell,
} from "@material-ui/core";

type SubscriptionPanelProps ={
  subscription: Subscription;
  symbolMode: boolean;
  updateAction: (newSubscription: Subscription) => void;
  deleteAction: (subscription: Subscription) => void;
  subscribeAction: (subscription: Subscription) => void;
}

export default function SubscriptionPanel(props: SubscriptionPanelProps){
  
  let [updatedThreshold, setUpdatedThreshold] = useState(props.subscription.threshold); 
  let [updatedPercentageThreshold, setUpdatedPercentageThreshold] = useState(props.subscription.percentageThreshold);
  let [updatedStartDate, setUpdatedStartDate] = useState<Date | null>(props.subscription.timeRange.startDate);
  let [updatedEndDate, setUpdatedEndDate] = useState<Date | null>(props.subscription.timeRange.endDate);
  let [updatedTrend, setUpdatedTrend] = useState<Trend>(props.subscription.trend);
  let newSubscription = {
    email: props.subscription.email, 
    symbol: props.subscription.symbol, 
    timeRange: {startDate: updatedStartDate, endDate: updatedEndDate} as TimeRange,
    threshold: updatedThreshold,
    percentageThreshold: updatedPercentageThreshold,
    trend: updatedTrend} as Subscription;

  let isUnchanged = updatedThreshold === props.subscription.threshold
   && updatedPercentageThreshold === props.subscription.percentageThreshold
   && updatedStartDate === props.subscription.timeRange.startDate
   && updatedEndDate === props.subscription.timeRange.endDate
   && updatedTrend === props.subscription.trend;

  let thresholdField = 
    <TextField
        defaultValue={props.subscription.threshold}
        id={props.subscription.symbol.symbol}
        label="Threshold"
        type="number"
        InputLabelProps={{
          shrink: true,
        }}
        variant="outlined"
        onChange={(e) => setUpdatedThreshold(Number(e.target.value))}
      />;

  let percentageThresholdField = 
      <TextField
          defaultValue={props.subscription.percentageThreshold}
          id={props.subscription.symbol.symbol}
          label="Percentage threshold"
          type="number"
          InputLabelProps={{
            shrink: true,
          }}
          variant="outlined"
          onChange={(e) => setUpdatedPercentageThreshold(Number(e.target.value))}
        />;

  let startDatePicker = 
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <DatePicker
        label="Start date"
        value={updatedStartDate}
        onChange={(newValue) => setUpdatedStartDate(newValue)}
        renderInput={(params) => <MuiTextField {...params} />}
      />
    </LocalizationProvider>;

  let endDatePicker = 
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <DatePicker
        label="End date"
        value={updatedEndDate}
        onChange={(newValue) => setUpdatedEndDate(newValue)}
        renderInput={(params) => <MuiTextField {...params} />}
      />
    </LocalizationProvider>;

  let trendPicker = 
    <FormControl fullWidth>
      <InputLabel>Trend</InputLabel> 
      <Select
        value={updatedTrend}
        label="Trend"
        onChange={e => setUpdatedTrend(e.target.value as Trend)}
      >
        <MenuItem value={Trend.Unspecified}>{Trend[Trend.Unspecified]}</MenuItem>
        <MenuItem value={Trend.Up}>{Trend[Trend.Up]}</MenuItem>
        <MenuItem value={Trend.Down}>{Trend[Trend.Down]}</MenuItem>
      </Select>
    </FormControl>;
    

  return (
    <TableRow>
      <TableCell>{props.subscription.symbol.baseAsset}</TableCell>
      <TableCell>{thresholdField}</TableCell>
      <TableCell>{percentageThresholdField}</TableCell>
      <TableCell>{startDatePicker}</TableCell>
      <TableCell>{endDatePicker}</TableCell>
      <TableCell>{trendPicker}</TableCell>
      {props.symbolMode
        ? <TableCell>
            <Button 
              disabled={isUnchanged}
              onClick={() => props.subscribeAction(newSubscription)}>
              Subscribe
            </Button>
          </TableCell>
        : <>
        <TableCell>
          <Button 
            disabled={isUnchanged}
            onClick={() => props.updateAction(newSubscription)}>
            Update
          </Button>
        </TableCell>
        <TableCell>
          <Button 
            onClick={() => props.deleteAction(props.subscription)}>
            Unsubscribe
          </Button>
        </TableCell>
        </>}
    </TableRow>
  )
}
